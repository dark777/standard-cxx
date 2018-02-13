#include <cctype>
#include <iostream>

static std::string getFileType(const char *name)
{
    std::string filename = name;
    int pos = filename.rfind('.');
    if (pos != std::string::npos) {
        std::string suffix = filename.substr(pos);
        std::string result;
        for (size_t i=0; i < suffix.size(); i++) {
            result += (char)tolower(suffix[i]);
        }
        return result;
    }
    else
        return std::string("");
} 

int main()
{
  std::cout<<"\n\tFile extension: "<<getFileType("myfile.txt")<<"\n\n";
  return 0;
}
/*
 Output:
 File extension: .txt
 
 */