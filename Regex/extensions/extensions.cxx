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
  const std::regex pattern(".*[.](aspx|php|py|java|rb|d|mp3|H|h|C|c|h(?:h|pp|xx|\\++)|H(?:H|PP|XX|\\++)|c(?:c|pp|xx|\\++|ss)|C(?:C|PP|XX|\\++|SS)|h(?:tm|tml)|H(?:TM|TML))");
  
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
 const char* files[33] = {
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
                          "file32.css",
                          "file33.CSS"
                         };
    
  for(int i=0; i<33; i++)
   std::cout << extensions(files[i]).print();
    std::cout << "\n";
    
  return 0;   
}