#include <regex>
#include <iostream>

























int main()
{
 std::string input;
 std::regex integer("[[:digit:]]");
 
 //As long as the input is correct ask for another number
 while(true)
  {
   std::cout<<"\n\tPress \"q\"\n\tor Give me an integer: ";
   std::cin>>input;
   if(!std::cin || input == "q") break;
   
   if(regex_match(input,integer))std::cout<<"\n\t"<<input<<"\n\n";
   else
   std::cout<<"\n\tInvalid input\n\n";
  }
}