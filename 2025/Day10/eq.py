import re
from z3 import *


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


def solve_with_z3(pattern, index_lists, b):
    """
    Given:
      pattern: list[int] used only for dimension (len(pattern) rows)
      index_lists: for each variable x_j, the list of row indices where x_j appears
      b: RHS values per row

    Builds and solves:
      for each row r: sum_{j: r in index_lists[j]} x_j = b[r]
      x_j >= 0, x_j integer
      min sum_j x_j
    """
    if pattern is None or b is None:
        raise ValueError("Missing pattern or b vector.")

    dim = len(pattern)
    if len(b) != dim:
        raise ValueError("pattern and b lengths do not match.")

    n_vars = len(index_lists)

    # Integer variables x0 ... x_(n_vars-1)
    x = [Int(f"x{j}") for j in range(n_vars)]

    opt = Optimize()

    # Non-negativity
    for xj in x:
        opt.add(xj >= 0)

    # Row constraints
    for row in range(dim):
        vars_in_row = []
        for j, idxs in enumerate(index_lists):
            if row in idxs:
                vars_in_row.append(x[j])

        if vars_in_row:
            opt.add(Sum(vars_in_row) == b[row])
        else:
            # No variable in this row: enforce 0 = b[row]
            opt.add(0 == b[row])

    # Objective: minimize sum xi
    obj = Sum(x)
    opt.minimize(obj)

    # Solve
    result = opt.check()
    if result == sat:
        model = opt.model()
        solution = [model[xj].as_long() for xj in x]
        obj_val = model.eval(obj).as_long()
        return solution, obj_val
    elif result == unsat:
        return None, None
    else:  # unknown
        return None, None


if __name__ == "__main__":
    with open("input.in", "r", encoding="utf-8") as f:
        res = 0
        for line_no, line in enumerate(f, start=1):
            line = line.strip()
            if line == "":
                continue

            pattern, index_lists, b = parse_input(line)

            try:
                solution, obj_val = solve_with_z3(pattern, index_lists, b)
            except ValueError as e:
                print("Error:", e)
                continue

            if solution is None:
                print(f"===== Line {line_no} =====")
                print("No solution (unsat or unknown).")
            else:
                # print("min sum xi =", obj_val)
                res += obj_val

        print("grand total =", res)
