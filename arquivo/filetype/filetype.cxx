#include <cctype>
#include <iostream>

static std::string getFileType(const char *name)
{
 std::string filename = name;
 int pos = filename.rfind('.');
 
 if(pos != std::string::npos)
  {
   std::string suffix = filename.substr(pos);
   std::string result;
     
   for(std::size_t i=0; i < suffix.size(); i++)
    {
     result += (char)tolower(suffix[i]);
    }
   return result;
  }
  else
 return std::string("");
}

std::string extractExtension(const std::string& file_name)
{
 const std::string::size_type pos = file_name.find_last_of('.');
 return (pos == std::string::npos)? file_name : file_name.substr(pos);
}

int main()
{
  std::cout<<"\n\tFile extensions\n\t"<<extractExtension("myfile.txt")<<"\n\t"<<getFileType("myfile.txt")<<"\n\n";
  return 0;
}
/*
 Output:
 File extension: .txt
 
 */
