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
  const std::regex pattern(".*[.](h|h(?:h|pp|xx|\\++)|H|H(?:H|PP|XX|\\++)|c|c(?:c|pp|xx|\\++)|C|C(?:C|PP|XX|\\++)|h(?:tm|tml)|H(?:TM|TML)|aspx|php|py|java|rb|d|mp3|css)");
  
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
  const std::string files[32] = {
                                 "file1.c",
                                 "file2.cc",
                                 "file3.cpp",
                                 "file4.cxx",
                                 "file5.c++",
                                 "file6.C",
                                 "file7.CC",
                                 "file8.CPP",
                                 "file9.CXX",
                                 "file10.C++",
                                 "file11.h",
                                 "file12.hh",
                                 "file13.hpp",
                                 "file14.hxx",
                                 "file15.h++",
                                 "file16.H",
                                 "file17.HH",
                                 "file18.HPP",
                                 "file19.HXX",
                                 "file20.H++",
                                 "file21.htm",
                                 "file22.HTM",
                                 "file23.HTML",
                                 "file24.HTML",
                                 "file25.aspx",
                                 "file26.php",
                                 "file27.py",
                                 "file28.java",
                                 "file29.rb",
                                 "file30.d",
                                 "file31.mp3",
                                 "file32.css"
                                };
    for(int i=0; i<32; i++)
     std::cout << extensions(files[i]).print();
      std::cout << "\n";
   return 0;   
}