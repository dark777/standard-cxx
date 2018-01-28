#include <regex>
#include <iostream>

#define this (*this)

struct file
{ 
 file(std::string file): _file(file){}

 ~file()
  {
   if(_file.length() != 0)_file.clear();
  } 

  bool isFile()
  {
    std::smatch file_smatch;
    const std::regex pattern(".*[.](h|hpp|hxx|H|HPP|HXX|c|cpp|cxx|C|CPP|CXX|aspx|php|py|java|rb|d|htm|html|HTM|HTML|mp3|css)");
    return std::regex_match(this._file, file_smatch, pattern);
  }

 file* print()
 {
  std::cout<<"\n\tFile: "<<this._file<<(file(this._file).isFile()?"\n":" is Invalid\n");
 }

  private:
  std::string _file;
};

int main()
{
  const std::string files[15]={"file1.c",
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
    file(files[i]).print();
    std::cout<<"\n\n";
}