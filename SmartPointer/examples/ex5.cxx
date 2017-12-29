#include <iostream>

struct SmartPointer
{  
  SmartPointer(std::string *s, int *age):p_Str(s), p_Int(age){}
    
  ~SmartPointer() 
   {
    if((*p_Str).length() != 0)delete p_Str;
     
    if(p_Int != NULL)delete p_Int;
   }
    
  std::string *getString()
  {
   if(p_Str != NULL)return p_Str;
   else
   p_Str=NULL;
  }
   
  int *getInt()
  {
   if(p_Int != NULL)return p_Int;
   else
   p_Int=NULL;  
  }
  
 private:
  std::string *p_Str;
  int *p_Int;
};


int main()
{
  std::string str1, str2, strnome;
  int idade;
  
  std::cout<<"Enter First e Last Name: ";
  std::cin>>str1>>str2;
  
  std::cout<<"Enter Age: ";
  std::cin>>idade;
  
  strnome+=str1+" "+str2;
  
  SmartPointer sp(new std::string(strnome), new int(idade));
    
  std::cout << "\n\tUser: "<<*sp.getString()<<"\n\tUser Age: "<<*sp.getInt()<< " years\n\n";
  
  return 0;
}