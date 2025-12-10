import re
import numpy as np

def select_independent_columns(M, tol=1e-10):
    """
    M: (rows, cols) mátrix.
    Visszaadja azon oszlopindexek listáját, amelyek lineárisan függetlenek.
    Greedy: mindig csak akkor vesszük hozzá az új oszlopot,
    ha növeli a rangot.
    """
    rows, cols = M.shape
    independent = []
    current_rank = 0

    for j in range(cols):
        candidate_indices = independent + [j]
        sub = M[:, candidate_indices]
        r = np.linalg.matrix_rank(sub, tol=tol)
        if r > current_rank:
            independent.append(j)
            current_rank = r
            if current_rank == rows:  # ennél több független oszlop nem lehet
                break

    return independent

def select_independent_rows(M, tol=1e-10):
    """
    M: (rows, cols) mátrix.
    Visszaadja azon sorindexek listáját, amelyek lineárisan függetlenek.
    Greedy: mindig akkor vesszük hozzá az új sort, ha növeli a rangot.
    """
    rows, cols = M.shape
    independent = []
    current_rank = 0

    for i in range(rows):
        candidate_indices = independent + [i]
        sub = M[candidate_indices, :]
        r = np.linalg.matrix_rank(sub, tol=tol)
        if r > current_rank:
            independent.append(i)
            current_rank = r
            if current_rank == cols:  # ennél több független sor nem lehet
                break

    return independent


def parse_input(text):
    """
    Input pl.:
    [..##.#...#] (0,3,4,7,9) ... (0,2,6,7,8,9) {87,70,44,58,58,67,44,54,55,54}

    Visszaad:
      pattern: list[int] 0/1 a [] rész alapján ('.' -> 0, '#' -> 1)
      index_lists: list[list[int]] a () csoportokból (EZEK OSZLOPVEKTOROK)
      b: list[int] a {} részben lévő számok
    """
    # 1) pattern a []-ből
    pattern = None
    m = re.search(r"\[(.*?)\]", text)
    if m:
        raw = m.group(1).strip()
        pattern = [1 if ch == '#' else 0 for ch in raw if ch in ".#"]

    # 2) indexlisták a ()-kból (minden () egy oszlopvektor indexlistája)
    groups = re.findall(r"\((.*?)\)", text)
    index_lists = []
    for g in groups:
        g = g.strip()
        if g == "":
            index_lists.append([])
        else:
            index_lists.append([int(x.strip()) for x in g.split(",")])

    # 3) b vektor a {}-ből
    b = None
    m = re.search(r"\{(.*?)\}", text)
    if m:
        raw = m.group(1)
        b = [int(x.strip()) for x in raw.split(",") if x.strip() != ""]

    return pattern, index_lists, b


def indices_to_binary_column_matrix(index_lists, dim):
    """
    index_lists: list of index-lists, MINDEN lista EGY OSZLOPVEKTOR
                 i.-edik lista: mely sorok legyenek 1-esek az i.-edik oszlopban.
    dim: sorok száma (vektorhossz)

    Visszaad:
      M: np.array shape (dim, n_cols)
         ahol n_cols = len(index_lists)
    """
    n_cols = len(index_lists)
    M = np.zeros((dim, n_cols), dtype=float)

    for col in range(n_cols):
        idxs = index_lists[col]
        for row in idxs:
            M[row, col] = 1.0

    return M


def select_independent_columns(M, tol=1e-10):
    """
    M: (rows, cols) mátrix.
    Visszaad:
      independent_cols: lista azokról az oszlopindexekről,
                        amelyek lineárisan függetlenek.
    Greedy, rangnövekedés alapján.
    """
    rows, cols = M.shape
    independent = []
    
    for j in range(cols):
        # próbáljuk hozzávenni a j-edik oszlopot
        candidate_indices = independent + [j]
        candidate = M[:, candidate_indices]
        r = np.linalg.matrix_rank(candidate, tol=tol)
        if r > len(independent):
            independent.append(j)
            # ha már van rows darab független oszlopunk, meg is állhatunk
            if len(independent) == rows:
                break
    
    return independent



def make_square_A_and_Y(M, b, tol=1e-10):
    """
    M: (rows, cols) – oszlopvektorokból épített mátrix
    b: 1D np.array – jobb oldal

    Cél:
      - A: négyzetes, invertálható (r x r, ahol r = rank(M))
      - b_new: az A-hoz tartozó sorokból kivágott rész (hossz r)
      - Y: a maradék oszlopok listája, mindegyik 1D vektor (hossz r)

    Ha M rangja 0, feldob egy hibát.
    """
    rows, cols = M.shape
    b = np.asarray(b, dtype=float)

    # 1) max. számú lineárisan független oszlop
    indep_cols = select_independent_columns(M, tol=tol)
    r = len(indep_cols)

    if r == 0:
        raise ValueError("A mátrix rangja 0, nincs egyetlen lineárisan független oszlop sem.")

    # 2) Válasszunk sorokat is, hogy négyzetes legyen (r x r)
    M_col = M[:, indep_cols]              # shape: (rows, r)
    indep_rows = select_independent_rows(M_col, tol=tol)
    r2 = len(indep_rows)

    if r2 < r:
        # Elvileg nem nagyon kellene előforduljon, de legyünk védettek
        r = r2
        indep_cols = indep_cols[:r]
        M_col = M[:, indep_cols]
        indep_rows = select_independent_rows(M_col, tol=tol)

    # 3) Négyzetes A és hozzátartozó b
    A = M[np.ix_(indep_rows, indep_cols)]  # r x r
    b_new = b[indep_rows]                  # hossz r

    # 4) Y: minden olyan oszlop, ami NINCS benne indep_cols-ban
    Y = []
    indep_set = set(indep_cols)
    for j in range(cols):
        if j not in indep_set:
            Y.append(M[indep_rows, j].copy())   # csak a kiválasztott sorokat tartjuk meg

    return A, b_new, Y



def nonnegative_integer_vectors_with_sum(n, k):
    """
    Yieldeli az összes n-dimenziós nemnegatív egész vektort,
    amelynek az elemeinek összege pontosan k.
    """
    if n == 1:
        # csak egy koordináta marad, annak muszáj k-nak lennie
        yield (k,)
        return

    for i in range(k + 1):
        # az első koordináta = i
        # a maradék n-1 koordináta összege = k - i
        for rest in nonnegative_integer_vectors_with_sum(n - 1, k - i):
            yield (i,) + rest

def combinations(n):
    if n == 0:
        yield None
        return
    
    k = 0
    while True:
        for q in nonnegative_integer_vectors_with_sum(n, k):
            yield np.array([q])
        k += 1

if __name__ == "__main__":
    res = 0
    with open("input.in", "r", encoding="utf-8") as f:
        for line_no, line in enumerate(f, start=1):
            line = line.strip()
            if line == "":
                continue  # üres sor kihagyása

            print(f"\n===== {line_no}. sor =====")

            pattern, index_lists, b_list = parse_input(line)

            if pattern is None or b_list is None:
                print("  HIBA: Hiányzik a pattern vagy a b vektor")
                continue

            dim = len(pattern)
            M = indices_to_binary_column_matrix(index_lists, dim=dim)
            b = np.array(b_list, dtype=float)

            A, b_square, Y = make_square_A_and_Y(M, b)

            print("  M shape:", M.shape)
            print("  A shape:", A.shape)
            print("  b_square shape:", b_square.shape)
            print("  Y shape:", len(Y))

           
            # print("y", Y_mat)

            tol = 1e-9
            best = 1e9
            for idx, i in enumerate(combinations(len(Y))):
                # print("i", i.shape)
                if i is None:
                    q = b_square
                else:
                    i_y = (np.column_stack(Y) @ i.T).T
                    q = (b_square.T - i_y).T
                # print("i*y", i_y)
                # print("b_square", b_square)
                # print("q=", q)
                x = np.linalg.solve(A, q)
                # print(x)

                # ellenőrzés: minden elem >= 0?
                non_negative = np.all(x >= -tol)

                # ellenőrzés: minden elem egész szám?
                integers = np.all(np.abs(x - np.round(x)) < tol)
                # print(x)
                if non_negative and integers:
                    if i is not None:
                        s = np.sum(x)+ np.sum(i)
                    else:
                        s = np.sum(x)
                    if s < best:
                        best = s
                        print("  x =", x, "i=", i, "sum=", s, "res=", res) 

                if i is None or np.sum(i) >= best:
                    break

            res += best
    print(int(res))
