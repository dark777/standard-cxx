#include <iostream>
#include "range.hxx"

int main(int argc, char** argv)
{
    for (auto i : range(100))
    {
     std::cout << i << std::endl; // Prints 0 to 99
    }
    
    for (auto i : range(10, 100))
    {
        std::cout << i << std::endl; // Prints 10 to 99
    }
    
    for (auto i : range(10, 0, -1))
    {
        std::cout << i << std::endl; // Prints 10 to 1, counting down
    }
    
    for (auto i : range(2.718, 100.0, 3.14))
    {
        std::cout << i << std::endl; // Prints 2.178 to 96.918, by increments of π, yum!
    }
  return 0;   
}