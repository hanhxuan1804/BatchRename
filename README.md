# Info
- Họ và tên: Nguyễn Xuân Hạnh
- MSSV: 19120218

# BatchRename
## How To Run

**Chạy ứng dụng bằng cách chạy file BatchRename.exe trong thư mục Release**

### Prerequisites

*Nếu không thấy thư mục DLL trong thư mục chứa file BatchRename.exe vui lòng copy thư mục DLL từ thư mục Source code vào thư mục chứa file BatchRename.exe trong thư mục Release*


# Completed Things

## Core requirements 

1. Dynamically load all renaming rules from external DLL files
2. Select all files and folders you want to rename
3. Create a set of rules for renaming the files. 
    1. Each rule can be added from a menu 
    2. Each rule's parameters can be edited 
4. Apply the set of rules in numerical order to each file, make them have a new name
5. Save this set of rules into presets for quickly loading later if you need to reuse

### Renaming rules

1. Change the extension to another extension (no conversion, force renaming extension)
2. Add counter to the end of the file
3. Remove all space from the beginning and the ending of the filename
4. Replace certain characters into one character like replacing "-", "_" and " " into dot "."
5. Adding a prefix to all the files
6. Adding a suffix to all the files
7. Convert all characters to lowercase, remove all spaces
8. Convert filename to PascalCase

## Improvements

1. Drag & Drop a file to add to the list
2. Storing parameters for renaming using XML file / JSON / excel / database
3. Handling duplication
4. Last time state: When exiting the application, auto-save and load the 
    1. The current size of the screen
    2. Current position of the screen
    3. Last chosen preset
5. Using regular expressions
6. Let the user see the preview of the result
7. Using Ribbion Window
8. Automatically resize to fit application size


# Expected grade
***10***
