import re
import numpy as np


def parse_input(text):
    """
    Input pl.:
    [..##.#...#] (0,3,4,7,9) ... (0,2,6,7,8,9) {87,70,44,58,58,67,44,54,55,54}

    Visszaad:
      pattern: list[int] 0/1 a [] rész alapján ('.' -> 0, '#' -> 1)
      index_lists: list[list[int]] a () csoportokból (ezek MOST oszlopvektorok lesznek)
      b: list[int] a {} részben lévő számok
    """
    # 1) pattern a []-ből
    pattern = None
    m = re.search(r"\[(.*?)\]", text)
    if m:
        raw = m.group(1).strip()
        pattern = [1 if ch == "#" else 0 for ch in raw if ch in ".#"]

    # 2) indexlisták a ()-kból
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
                 az i. elem: mely sorok legyenek 1-esek az i. oszlopban
    dim: a vektorok hossza (sorok száma)

    Visszaad:
      M: np.array shape (dim, n_cols)
         ahol n_cols = len(index_lists)
    """
    n_cols = len(index_lists)
    M = np.zeros((dim, n_cols), dtype=float)

    for col, idxs in enumerate(index_lists):
        for row in idxs:
            M[row, col] = 1.0

    return M



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
    k = 0
    while True:
        for q in nonnegative_integer_vectors_with_sum(n, k):
            yield q
        k += 1

if __name__ == "__main__":
    input_str = "[..##.#...#] (0,3,4,7,9) (0,1,9) (1,2,3,4,5) (0,1,3,7,8) (1,3,4,5,6,7,9) (0,1,2,4,5,6,7,8) (0,1,2,3,5,6,8) (1,2,4,5,8,9) (0,4,5,6,7) (0,2,3,5,8,9) (0,2,6,7,8,9) {87,70,44,58,58,67,44,54,55,54}"
    # input_str = "[..###.#..#] (1,2,3,4,5,6,8,9) (1,6) (1,2,5,7) (1,3,6) (0,3,4,5,6,9) (1,4,6,8) (2,3,4,5,6,7) (0,2,4,5,8) (0,2,4,5,9) (0,1,2,6,7,9) (1,3,5,9) {30,66,40,50,46,42,78,22,28,35}"

    pattern, index_lists, b_list = parse_input(input_str)

    if pattern is None or b_list is None:
        raise ValueError("Pattern vagy b hiányzik az inputból")

    dim = len(pattern)  # ez legyen 10
    b = np.array(b_list, float)  # ez is legyen hossz 10

    # ellenőrzések, hogy pont azt kapjuk, amit akarsz:
    if len(b) != dim:
        raise ValueError(
            f"Elvárt len(b) == len(pattern), de len(b)={len(b)}, len(pattern)={dim}"
        )
    if len(index_lists) != dim + 1:
        raise ValueError(
            f"Elvárt 11 oszlopvektor (len(index_lists) == len(pattern)+1), "
            f"de len(index_lists)={len(index_lists)}, len(pattern)={dim}"
        )

    # 11 oszlopvektor -> 10x11-es mátrix
    M = indices_to_binary_column_matrix(index_lists, dim=dim)
    # M shape: (10, 11)

    # Szétbontás:
    y = M[:, 0]  # első oszlop: hossz 10
    A = M[:, 1:]  # maradék 10 oszlop: 10x10

    print("M shape:", M.shape)  # (10, 11)
    print("y shape:", y.shape)  # (10,)
    print("A shape:", A.shape)  # (10, 10)
    print("b shape:", b.shape)  # (10,)

    print("\nM =\n", M)
    print("\ny =", y)
    print("\nA =\n", A)
    print("\nb =", b)

    # for i in range(0, 1000): 
    #     x = np.linalg.solve(A, b-i * y) 
    #     print(f"{i} {x}")
        

    tol = 1e-9
    i = 0

    while True:
        rhs = b - i * y
        x = np.linalg.solve(A, rhs)

        # ellenőrzés: minden elem >= 0?
        non_negative = np.all(x >= -tol)

        # ellenőrzés: minden elem egész szám?
        integers = np.all(np.abs(x - np.round(x)) < tol)

        if non_negative and integers:
            break
        i+=1

    # ha ide jutunk, akkor x jó
    x_int = np.round(x).astype(int)
    print(f"i = {i}, x = {x_int}")

    i += 1
