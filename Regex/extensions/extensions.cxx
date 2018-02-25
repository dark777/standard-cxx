#include <regex>
#include <iostream>

struct extensions
{ 
 extensions(std::string file): _file(file){}

 ~extensions()
  {
   if(_file.length() != 0)_file.clear();
  }
  
 bool isFile()
 {
  std::smatch extensions_smatch;
  //validas arquivos com as extensoes da regex
  const std::regex pattern(".*[.](h|hpp|hxx|H|HPP|HXX|c|cpp|cxx|C|CPP|CXX|aspx|php|py|java|rb|d|htm|html|HTM|HTML|mp3|css)");
  
  return std::regex_match(_file, extensions_smatch, pattern);
 }

 std::string print()
 {
  return "\n\tFile: "+_file+(isFile()?" is Valid\n":" is Invalid\n");
 }

 private:
  std::string _file;
};

int main()
{
  const std::string files[15] = {
                                 "file1.c",
                                 "file2.cpp",
                                 "file3.cxx",
                                 "file4.C",
                                 "file5.CPP",
                                 "file6.CXX",
                                 "file7.h",
                                 "file8.hh",
                                 "file9.hpp",
                                 "file10.hxx",
                                 "file11.H",
                                 "file12.HH",
                                 "file13.HPP",
                                 "file14.HXX",
                                 "file15.aa"
                                };
    for(int i=0; i<15; i++)
     std::cout << extensions(files[i]).print();
      std::cout << "\n";
   return 0;   
}