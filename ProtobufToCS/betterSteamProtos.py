from pathlib import Path
import os
import shutil

source_folder = os.getcwd()+"\\backup\\"
destination_folder = os.getcwd()+"\\"

for file_name in os.listdir(source_folder):
    # construct full file path
    source = source_folder + file_name
    destination = destination_folder + file_name
    # copy only files
    if os.path.isfile(source):
        shutil.copy(source, destination)
        print('copied', file_name)

        
xdict = {}
namescsv = open("names.csv", "r").readlines()
for name in namescsv:
    name = name.replace("\n","")
    x = name.split(";")
    xdict[x[0]] = x[1]
    #print(xdict)

pathlist = Path(os.getcwd()).rglob('*.proto')
for path in pathlist:
     # because path is object not string
     path_in_str = str(path)
     if path_in_str.__contains__("google"):
         continue
     if path_in_str.__contains__("backup"):
         continue
     pkgname = path_in_str.replace(os.getcwd() + "\\","")
     pkgname = pkgname.replace(".proto","")
     tmp = open(path_in_str, "r").readlines()
     x = open(path_in_str, "w")
     #print(tmp)
     x.seek(0)
     x.write("syntax = \"proto2\";")
     x.write("\noption csharp_namespace=\"" + xdict[pkgname] + "\";\n")
     for t in tmp:
         if t.__contains__("(force_php_generation)"):
             continue
         x.write(t)
     x.close()
     print(pkgname)
