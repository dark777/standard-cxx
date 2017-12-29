#include <iostream>

class StringSmartPointer
{
 std::string *pStr;
    
   public:
   
    StringSmartPointer(std::string *s):pStr(s){}
    
   ~StringSmartPointer() 
    {
      delete pStr;
    }
    
   std::string *getString()
   {
    return pStr;
   }
};


int main()
{
    StringSmartPointer str(new std::string("Testando Smart Pointers"));
    
    std::cout << "StringSmartPointer: "<<*str.getString()<< "\n\n";
}
