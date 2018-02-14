#include <iostream>

//base class
struct validation
{
 validation(){}
 ~validation(){}
 //pure virtual function to enforce reimplementation
 virtual bool isValid() = 0;
};

struct gmail: validation
{
 gmail(std::string mail): _mail(mail){}
 
 ~gmail()
  {
   if(!_mail.empty())_mail.clear(); 
  }
  
  bool isValid()
  {
   std::string servidor("@gmail.com");
   
   std::string pedaco(_mail.substr(_mail.size()-servidor.size()));
   
   return !pedaco.compare(servidor);
  }
  
 private:
  std::string _mail; 
};

struct uol: validation
{
 uol(std::string mail): _mail(mail){}
 
 ~uol()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
  bool isValid()
  {
   std::string servidor("@uol.com.br");
   
   std::string pedaco(_mail.substr(_mail.size()-servidor.size()));
   
   return !pedaco.compare(servidor);
  }
  
 private:
  std::string _mail; 
};

struct bol: validation
{
 bol(std::string mail): _mail(mail){}
 
 ~bol()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
 bool isValid()
 {
  std::string servidor("@bol.com.br");
  
  std::string pedaco(_mail.substr(_mail.size()-servidor.size()));
  
  return !pedaco.compare(servidor);
 }
 
 private:
  std::string _mail; 
};

struct terra: validation
{
 terra(std::string mail): _mail(mail){}
 
 ~terra()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
 bool isValid()
 {
  std::string servidor("@terra.com.br");
  
  std::string pedaco(_mail.substr(_mail.size()-servidor.size()));
  
  return !pedaco.compare(servidor);
 }
 
 private:
  std::string _mail; 
};

struct yahoo: validation
{
 yahoo(std::string mail): _mail(mail){}
 
 ~yahoo()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
 bool isValid()
 {
  std::string servidor("@yahoo.com.br");
  
  std::string pedaco(_mail.substr(_mail.size()-servidor.size()));
  
  return !pedaco.compare(servidor);
 }
 
 private:
  std::string _mail; 
};

struct hotmail: validation
{
 hotmail(std::string mail): _mail(mail){}
 
 ~hotmail()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
 bool isValid()
 {
  std::string servidor("@hotmail.com.br");
  
  std::string pedaco(_mail.substr(_mail.size()-servidor.size()));
  
  return !pedaco.compare(servidor);
 }

 private:
  std::string _mail; 
};

void menu()
{
 int op;
 std::string str;
 validation* is_val[6];
 
 enum { Gmail=1, Uol, Bol, Terra, Yahoo, Hotmail, Exit };
 
 do{
    std::cout << "\n\tValidation e-mail"
                 "\n\t[1]-Gmail"
                 "\n\t[2]-Uol"
                 "\n\t[3]-Bol"
                 "\n\t[4]-Earth"
                 "\n\t[5]-Yahoo"
                 "\n\t[6]-Hotmail"
                 "\n\t[7]-Exit"
                 "\n\tEnter: ";
    std::cin>>op;
    
    switch(op)
     {      
      case Gmail:
      
      do{
         std::cout<<"\n\tEnter your gmail: ";
         std::cin>>str;
          
         is_val[0] = new gmail(str);
         
         if(is_val[0]->isValid())
         std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
         else 
         std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
        }while(!is_val[0]->isValid());
      
      break;
      
      case Uol:
      
      do{
         std::cout<<"\n\tEnter your uol mail: ";
         std::cin>>str;
         
         is_val[1] = new uol(str);
         
         if(is_val[1]->isValid())
         std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
         else 
         std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
        }while(!is_val[1]->isValid());
      
      break;
      
      case Bol:
      
      do{
         std::cout<<"\n\tEnter your bol mail: ";
         std::cin>>str;
         
         is_val[2] = new bol(str);
         
         if(is_val[2]->isValid())
         std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
         else 
         std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
        }while(!is_val[2]->isValid());
      
      break;
      
      case Terra:
      
      do{
         std::cout<<"\n\tEnter your earth mail: ";
         std::cin>>str;
         
         is_val[3] = new terra(str);
         
         if(is_val[3]->isValid())
         std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
         else 
         std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
        }while(!is_val[3]->isValid());
      
      break;
      
      case Yahoo:
      
      do{
         std::cout<<"\n\tEnter your yahoo mail: ";
         std::cin>>str;
         
         is_val[4] = new yahoo(str);
         
         if(is_val[4]->isValid())
         std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
         else 
         std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
         
        }while(!is_val[4]->isValid());
      
      break;
      
      case Hotmail:
      
      do{
         std::cout<<"\n\tEnter your hotmail: ";
         std::cin>>str;
         
         is_val[5] = new hotmail(str);
         
         if(is_val[5]->isValid())
         std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
         else 
         std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
        }while(!is_val[5]->isValid());
      
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