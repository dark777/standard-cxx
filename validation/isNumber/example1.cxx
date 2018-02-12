#include <sstream>
#include <iostream>

bool isOk(std::string x)
{
 std::string s;
 
 while(s.size() > 1 && x[0] == ' ')x.erase(x.begin());
 
 int ini = x[0] == '-';

 for(int i=ini;i<x.size();i++)
  if('0' <= x[i] && x[i] <= '9')
   continue;
   else
   return false;
   return true;
}

int toNumber(std::string x)
{
 std::string s;
 while(s.size() > 1 && x[0] == ' ')
  x.erase(x.begin());
    
  int ini = x[0] == '-', res = 0;
    
  for(int i=ini;i<x.size();i++)
   {
    res *= 10;
    res += x[i] - '0';
   } 
 return ini?-res:res;
}

int isNumber(std::string str)
{
  while(getline( std::cin, str )) 
  {
   const char *idx = str.c_str( );
   
   // Mientras que no lleguemos al final de la cadena,
   // y el caracter sea un dígito.
   while( *idx && *idx >= '0' && *idx <= '9' )++idx;
    
   // Si llegamos al final de la cadena, la validación es correcta.
   if(!( *idx ) )break;
   
   std::cout << "\n\tEntrada inválida.\n"; 
  }
} 

void getNum1()
{
 std::string s;
 do{
    std::cout<<"\n\tDigite um numero: ";
    std::cin >> s;
    
    if(isOk(s))
     std::cout << "\n\tIs Number " << toNumber(s) << "\n\n";
    else
     std::cout << "\n\tIs Invalid.\n\n";
    
   }while(isOk(s) == 0);
}

void getNum2()
{
 std::string s;
 do{
    std::cout<<"\n\tDigite um numero: ";
    std::cin >> s;
    
    if(isOk(s))
     std::cout << "\n\tIs Number " << s << "\n\n";
    else
     std::cout << "\n\tIs Invalid.\n\n";
    
   }while(isOk(s) == 0);
}

bool checkNumber(std::string &s) 
{
 std::istringstream ssIn(s);
 int n;
 
 if(ssIn >> n)
  {
   //std::cout << n << std::endl;
   // we got a number, feed it back
   std::stringstream ssOut;
   ssOut << n;
   s = ssOut.str();	  
   return true;
  }
  else
  s.clear();
 return false;
}

void getBool()
{
 std::string num;
 
 do{ 
    std::cout<<"\n\tDigite um numero: ";
    std::cin>>num;
  
    if(checkNumber(num) == 0)
     std::cout<<"\n\tNao é um numero\n\n";
    else
     std::cout<<"\n\tÉ um numero.\n\n";
   }while(checkNumber(num) == 0);  
}

int main()
{
 //getNum2();
  getBool();
 return 0;
}