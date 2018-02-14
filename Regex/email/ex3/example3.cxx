#include <iostream>

bool gmail(std::string email)
{
 std::string servidor("@gmail.com");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

bool uol(std::string email)
{
 std::string servidor("@uol.com.br");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

bool bol(std::string email)
{
 std::string servidor("@bol.com.br");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

bool terra(std::string email)
{
 std::string servidor("@terra.com.br");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

bool yahoo(std::string email)
{
 std::string servidor("@yahoo.com.br");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

bool hotmail(std::string email)
{
 std::string servidor("@hotmail.com.br");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

void menu()
{
 int op;
 std::string str;
 enum Mail{ Gmail=1, Uol, Bol, Terra, Yahoo, Hotmail, Exit };
 
 do{
    std::cout << "\n\tValidation Email"
                 "\n\t[1]-Gmail"
                 "\n\t[2]-Uol"
                 "\n\t[3]-Bol"
                 "\n\t[4]-Terra"
                 "\n\t[5]-Yahoo"
                 "\n\t[6]-Hotmail"
                 "\n\t[7]-Exit"
                 "\n\tEnter: ";
    std::cin>>op;
    
     switch(op)
     {      
      case Gmail:
       std::cout<<"\n\tEnter your email: ";
       std::cin>>str;
       
       if(gmail(str))
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
       else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
      break;
      
      case Uol:
       std::cout<<"\n\tEnter your email: ";
       std::cin>>str;
       
       if(uol(str))
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
       else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
      break;
      
      case Bol:
       std::cout<<"\n\tEnter your email: ";
       std::cin>>str;
       
       if(bol(str))
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
       else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
      break;
      
      case Terra:
       std::cout<<"\n\tEnter your email: ";
       std::cin>>str;
       
       if(terra(str))
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
       else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
      break;
      
      case Yahoo:
       std::cout<<"\n\tEnter your email: ";
       std::cin>>str;
       
       if(yahoo(str))
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
       else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
      break;
      
      case Hotmail:
       std::cout<<"\n\tEnter your email: ";
       std::cin>>str;
       
       if(hotmail(str))
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
       else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
      break;
                                    
      case Exit:
       std::cout << "\n\tGood Bye!\n\n";
       exit(1);
      break;
      
      default:
       std::cout << "\n\tInvalid Option!\n\n";
     }
   }while(1); 
}

int main() 
{
 menu();
 return 0;
}