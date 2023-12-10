import os
import re

def is_valid_directory(directory_name):
    # Use regular expression to check if the directory name ends with 'year/dayxx'
    pattern = r'\d{4}/Day\d{2}$'
    return re.findall(pattern, directory_name) != []


def print_first_two_paragraphs(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        lines = file.readlines()

        # Find the index of the first empty line, indicating the end of the third paragraph
        empty_line_indices = [i for i, line in enumerate(lines) if line.strip() == '']
        if len(empty_line_indices) >= 2:
            third_paragraph_start = empty_line_indices[1] + 1
        else:
            third_paragraph_start = len(lines)

        # Print the first three paragraphs
        return ''.join(lines[:third_paragraph_start])


def process_readme_files(directory_path):
    for root, dirs, files in os.walk(directory_path):
        if  not is_valid_directory(root):
            continue

        for file_name in files:
            if file_name.lower() == 'readme.md':
                print(root + '/' + file_name)
                file_path = os.path.join(root, file_name)
                ps = print_first_two_paragraphs(file_path)  
                ps = ps.splitlines();
                head = ps[0]
                ps = ps[1:]
                pattern = r'\[(.*?)\]'
                link = re.findall(pattern, head)
                ps.append(f'Read the [full puzzle]({link[0]}).');
                ps = '\n'.join(ps)

                with open(root + '/' + file_name, 'w') as file:
                    file.write(ps)


# Replace 'your_directory_path' with the actual path to your directory
directory_path = '/Users/encse/projects/adventofcode'
process_readme_files(directory_path)