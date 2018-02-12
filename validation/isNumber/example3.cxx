#include <regex>
#include <iostream>
//Real number

bool isReal(std::string input)
{
 std::regex rr("((\\+|-)?[[:digit:]]+)[.](([[:digit:]]+)?)?((e|E)((\\+|-)?)[[:digit:]]+)?");
 //As long as the input is correct ask for another number
 
 while(true)
  {
   std::cout<<"Digite um numero real(float or double): ";
   std::cin>>input;
   
   //Exit when the user inputs q
   if(input == "q")break;
   
   if(std::regex_match(input,rr))std::cout<<"\n\tNumber: "<<input<<" is Float.\n\n";
   else
   std::cout<<"\n\tInvalid input\n\n";
  }
}

int main()
{
 std::string input;
 
 while(!isReal(input));
 
 return 0;
}