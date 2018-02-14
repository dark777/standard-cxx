#include <map>
#include <iterator>
#include <iostream>

std::map<std::string, int> months;
std::map<std::string, int>::iterator iter;

int GetValue(std::string vals)
{
 return months[vals];
}

std::string GetName(int value)
{
 for(iter = months.begin(); iter != months.end(); iter++ )
  if( value == iter->second )
   return (std::string)iter->first;
    return "Illegal value";   //Error Illegal input
}

void initMonths()
{
 months["January"]  =  1;
 months["February"] =  2;
 months["March"]    =  3;
 months["April"]    =  4;
 months["May"]      =  5;
 months["June"]     =  6;
 months["July"]     =  7;
 months["August"]   =  8;
 months["Setember"] =  9;
 months["October"]  = 10;
 months["November"] = 11;
 months["December"] = 12; 
}
void getMont(std::string name, int value)
{
 initMonths();
    
 std::string num_mont(std::to_string(GetValue(name)));

 std::cout<<"\n\t"<<std::to_string(GetValue(name))
          <<"\n\t"<<GetName(value)
          <<"\n\n";
}

int main()
{
 int num; 
 std::string str;
 
 std::cout<<"Digite o numero de um mês: ";
 std::cin>>num;
 
 std::cout<<"Digite o nome de um mês: ";
 std::cin>>str;
 
 getMont(str, num);
}