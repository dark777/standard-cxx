#include <iostream>

//Static polymorphism

struct Base
{
  virtual ~Base() { }
  virtual void method() { std::cout << "Base"; }
};

struct Derived : Base
{
 virtual void method() 
 { 
   std::cout << "\n Derived\n\n"; 
 }
};

int main()
{
    Base *pBase = new Derived;
    pBase->method(); //outputs "Derived"
    delete pBase;
    return 0;
} 
