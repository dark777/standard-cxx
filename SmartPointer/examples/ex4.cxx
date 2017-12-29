#include <iostream>


class Person
{
 int age;
 const char* pName;
 std::string* sName; 

  public:
   Person(): pName(0),sName(0),age(0){}
   
   Person(const char* pName, int age): pName(pName), age(age){}
   
   Person(std::string* sName, int age): sName(sName), age(age){}
   
   ~Person(){}

   void display()
   {
    if(sName->length() != 0)
      std::cout<<"\n\tName: "<<*sName<<" Age: "<<age<<"\n\n";
    else
      std::cout<<"\n\tName: "<<pName<<" Age: "<<age<<"\n\n";
   }
        
   void Shout()
   {
    printf("Ooooooooooooooooo");
   } 
};

template < typename T >
class GenericSmartPointer
{
 T* pData; // Generic pointer to be stored
         
 public:
    
  GenericSmartPointer(T* pValue) : pData(pValue){}

  ~GenericSmartPointer()
   {
    delete pData;
   }

   T& operator* ()
   {
    return *pData;
   }

   T* operator-> ()
   {
    return pData;
   }
};

int main()
{
  int idade;
  std::string str;
  
  std::cout<<"Digite um nome e idade: ";
  std::cin>>str>>idade;
  
  GenericSmartPointer<Person> gsp(new Person(new std::string(str), idade));
  gsp->display(); // Não precisa apagar o ponteiro da pessoa.
}