import re
from fractions import Fraction
from itertools import product
import sys


def parse_input(text):
    """
    Example input:
      [..##.#...#] (0,3,4,7,9) ... (0,2,6,7,8,9) {87,70,44,58,58,67,44,54,55,54}

    Returns:
      pattern: list[int] 0/1 from [] section ('.' -> 0, '#' -> 1)
      index_lists: list[list[int]] from () groups (each list is a column vector index list)
      b: list[int] from {} section
    """
    # 1) pattern from []
    pattern = None
    m = re.search(r"\[(.*?)\]", text)
    if m:
        raw = m.group(1).strip()
        pattern = [1 if ch == '#' else 0 for ch in raw if ch in ".#"]

    # 2) index lists from () groups
    groups = re.findall(r"\((.*?)\)", text)
    index_lists = []
    for g in groups:
        g = g.strip()
        if g == "":
            index_lists.append([])
        else:
            index_lists.append([int(x.strip()) for x in g.split(",")])

    # 3) b vector from {}
    b = None
    m = re.search(r"\{(.*?)\}", text)
    if m:
        raw = m.group(1)
        b = [int(x.strip()) for x in raw.split(",") if x.strip() != ""]

    return pattern, index_lists, b


# ----------------- Linear algebra helpers ----------------- #

def build_matrix_from_index_lists(num_rows, index_lists):
    """
    Given:
      num_rows: number of rows (length of b or pattern)
      index_lists[j]: list of row indices affected by button j

    Returns:
      A as a list of lists, shape (num_rows x num_buttons)
      with A[i][j] = 1 if i in index_lists[j] else 0
    """
    num_cols = len(index_lists)
    A = [[0 for _ in range(num_cols)] for _ in range(num_rows)]
    for j, idxs in enumerate(index_lists):
        for i in idxs:
            A[i][j] = 1
    return A


def find_basis_and_kernel_columns(A):
    """
    Run Gaussian elimination on A (row ops only) to find pivot columns
    and the corresponding pivot rows (in the ORIGINAL row numbering).

    Works for any m x n matrix A, including tall matrices (more rows than columns).

    Returns:
      pivot_cols: list[int]   - column indices that form a basis (linearly independent)
      kernel_cols: list[int]  - remaining column indices (kernel directions)
      pivot_rows: list[int]   - original row indices that contain pivots
    """
    m = len(A)
    n = len(A[0])

    M = [[Fraction(A[i][j]) for j in range(n)] for i in range(m)]
    row_perm = list(range(m))  # tracks original row indices

    pivot_cols = []
    pivot_rows = []

    row = 0
    for col in range(n):
        if row >= m:
            break

        # Find pivot row at or below 'row' with nonzero in this column
        pivot_row = None
        for r in range(row, m):
            if M[r][col] != 0:
                pivot_row = r
                break
        if pivot_row is None:
            continue  # no pivot in this column

        # Swap current row with pivot_row
        if pivot_row != row:
            M[row], M[pivot_row] = M[pivot_row], M[row]
            row_perm[row], row_perm[pivot_row] = row_perm[pivot_row], row_perm[row]

        pivot_cols.append(col)
        pivot_rows.append(row_perm[row])

        # Eliminate below
        pivot_val = M[row][col]
        for r in range(row + 1, m):
            if M[r][col] != 0:
                factor = M[r][col] / pivot_val
                for c in range(col, n):
                    M[r][c] -= factor * M[row][c]

        row += 1

    kernel_cols = [j for j in range(n) if j not in pivot_cols]
    return pivot_cols, kernel_cols, pivot_rows


def solve_square_fraction(B, rhs):
    """
    Solve B x = rhs where B is n x n, rhs is length n.
    All entries are ints or Fractions. Uses Gaussian elimination with Fractions.

    Returns:
      x as a list of Fraction, or None if system is singular or inconsistent.
    """
    n = len(B)
    if n == 0:
        # 0x0 system: treat as trivially solvable, x = []
        return []

    # Build augmented matrix
    M = [[Fraction(B[i][j]) for j in range(n)] + [Fraction(rhs[i])] for i in range(n)]

    col = 0
    for row in range(n):
        if col >= n:
            break

        # find pivot
        pivot_row = None
        for r in range(row, n):
            if M[r][col] != 0:
                pivot_row = r
                break
        if pivot_row is None:
            col += 1
            row -= 1  # compensate for row++ in loop
            continue

        # swap
        if pivot_row != row:
            M[row], M[pivot_row] = M[pivot_row], M[row]

        # normalize pivot row
        pivot_val = M[row][col]
        for c in range(col, n + 1):
            M[row][c] /= pivot_val

        # eliminate below
        for r in range(row + 1, n):
            if M[r][col] != 0:
                factor = M[r][col]
                for c in range(col, n + 1):
                    M[r][c] -= factor * M[row][c]

        col += 1

    # Back substitution
    x = [Fraction(0) for _ in range(n)]
    for i in reversed(range(n)):
        # Find leading coefficient in row i
        lead_col = None
        for j in range(n):
            if M[i][j] != 0:
                lead_col = j
                break

        if lead_col is None:
            # Entire row of zeros: check consistency
            if M[i][n] != 0:
                return None  # inconsistent
            continue

        val = M[i][n]
        for j in range(lead_col + 1, n):
            if M[i][j] != 0:
                val -= M[i][j] * x[j]
        if M[i][lead_col] == 0:
            if val != 0:
                return None
        else:
            x[lead_col] = val / M[i][lead_col]

    return x


def generate_y_vectors(dim, total):
    """
    Yield all non-negative integer vectors y of length dim with sum(y) == total.
    """
    if dim == 1:
        yield [total]
        return
    for first in range(total + 1):
        for rest in generate_y_vectors(dim - 1, total - first):
            yield [first] + rest


def check_solution(A, b, x):
    """
    Verify if A x == b for integer vector x.
    """
    m = len(A)
    n = len(A[0])
    for i in range(m):
        s = 0
        row = A[i]
        for j in range(n):
            if row[j]:
                s += row[j] * x[j]
        if s != b[i]:
            return False
    return True


def find_min_nonnegative_solution(A, b, max_total_sum=200):
    """
    Implements Gauss-elimination + kernel brute-force.

    Handles:
      - general m x n matrices
      - tall matrices (m > n) via full column-rank pivot selection
      - small kernel dimension (<= 2-3) by brute-forcing non-negative y

    Returns:
      best_solution: list[int] of length n with minimal sum(x_i), or None.
    """
    m_rows = len(A)
    n_cols = len(A[0])

    if len(b) != m_rows:
        raise ValueError("Dimensions mismatch: len(b) != number of rows in A")

    pivot_cols, kernel_cols, pivot_rows = find_basis_and_kernel_columns(A)
    rank = len(pivot_cols)
    k_dim = len(kernel_cols)

    # Handle completely zero matrix
    if rank == 0:
        # Ax = 0, so we need b == 0 for any solution; we choose x = 0
        if any(val != 0 for val in b):
            return None
        return [0 for _ in range(n_cols)]

    # Build reduced system on pivot rows and pivot columns:
    # B (rank x rank) and K (rank x k_dim)
    B = [[A[row][col] for col in pivot_cols] for row in pivot_rows]
    b_red = [b[row] for row in pivot_rows]

    K = [[A[row][col] for col in kernel_cols] for row in pivot_rows]
    K_frac = [[Fraction(K[i][j]) for j in range(k_dim)] for i in range(rank)]
    b_frac = [Fraction(x) for x in b_red]

    best_solution = None
    best_total = None

    # Special case: no kernel => unique real solution for base variables
    if k_dim == 0:
        x_base = solve_square_fraction(B, b_frac)
        if x_base is None:
            return None

        x_full = [0 for _ in range(n_cols)]
        # Check integrality & non-negativity
        for idx, col in enumerate(pivot_cols):
            val = x_base[idx]
            if val < 0 or val.denominator != 1:
                return None
            x_full[col] = int(val)

        # Verify all rows
        if not check_solution(A, b, x_full):
            return None

        return x_full

    # General case: kernel dimension > 0
    for total in range(0, max_total_sum + 1):
        for y_vec in generate_y_vectors(k_dim, total):
            # rhs = b_red - K * y
            rhs = b_frac.copy()
            for j in range(k_dim):
                yj = y_vec[j]
                if yj != 0:
                    for i in range(rank):
                        rhs[i] -= K_frac[i][j] * yj

            # Solve B x_base = rhs
            x_base = solve_square_fraction(B, rhs)
            if x_base is None:
                continue

            # Check integrality and non-negativity of x_base
            valid = True
            x_base_int = [0 for _ in range(rank)]
            for i, val in enumerate(x_base):
                if val < 0 or val.denominator != 1:
                    valid = False
                    break
                x_base_int[i] = int(val)
            if not valid:
                continue

            # Build full x (length n_cols)
            x_full = [0 for _ in range(n_cols)]
            for idx, col in enumerate(pivot_cols):
                x_full[col] = x_base_int[idx]
            for idx, col in enumerate(kernel_cols):
                x_full[col] = y_vec[idx]

            # Verify that this x actually satisfies ALL rows of Ax = b
            if not check_solution(A, b, x_full):
                continue

            total_presses = sum(x_full)

            if best_total is None or total_presses < best_total:
                best_total = total_presses
                best_solution = x_full

        # termination condition: once we've enumerated y with sum >= best_total,
        # we can't find a better solution (sum(x) >= sum(y) since x is non-negative)
        if best_total is not None and total >= best_total:
            break

    return best_solution


# ----------------- Advent of Code glue ----------------- #

def solve_from_line(line):
    """
    Glue for one AoC input line:

      - parse line
      - build A from index_lists
      - optionally adjust b using pattern
      - find minimal non-negative integer solution x
      - RETURN sum(x), since AoC answer is total button presses
    """
    pattern, index_lists, b = parse_input(line)

    if b is None or index_lists is None:
        raise ValueError("Input line did not contain both () groups and {} group")

    m = len(b)

    # If pattern influences b (e.g. initial state), adjust here, e.g.:
    # if pattern is not None:
    #     b_effective = [b[i] - pattern[i] for i in range(m)]
    # else:
    #     b_effective = b
    #
    # For now, assume b is already the right-hand side:
    b_effective = b

    A = build_matrix_from_index_lists(m, index_lists)

    x = find_min_nonnegative_solution(A, b_effective, max_total_sum=200)
    if x is None:
        return None  # or raise, depending on how you want to signal "no solution"

    return sum(x)

if __name__ == "__main__":

     sys.stdout.reconfigure(line_buffering=True)

     with open("input.in", "r", encoding="utf-8") as f:
        res = 0
        for line_no, line in enumerate(f, start=1):
            line = line.strip()
            if line == "":
                continue

            sol = solve_from_line(line)
            print("min sum xi =", sol)
            res += sol
        print("grand total =", res)
