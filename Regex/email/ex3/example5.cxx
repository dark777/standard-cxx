#include <iostream>

//base class
struct validation
{
 validation(){}
 ~validation(){}
 //pure virtual function to enforce reimplementation
 virtual void menu() = 0;
 virtual bool isValid() = 0;
};

void menu_All();

int main() 
{
 menu_All(); 
 return 0;
}

struct gmail: validation
{
 gmail(){}
 
 gmail(std::string mail): _mail(mail){}
 
 ~gmail()
  {
   if(!_mail.empty())_mail.clear(); 
  }
    
  void menu()
  {
   do{
      std::cout<<"\n\tEnter your google e-mail: ";
      std::cin>>str;
          
      is_val = new gmail(str);
         
      if(is_val->isValid())
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
      else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
          
     }while(!is_val->isValid());
  }
   
 private:
  
  bool isValid()
  {
   std::string server("@gmail.com");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
  }
  
  std::string str;
  validation* is_val;
  std::string _mail; 
};

struct uol: validation
{
 uol(){}
 
 uol(std::string mail): _mail(mail){}
 
 ~uol()
  {
   if(!_mail.empty())_mail.clear();  
  }
    
  void menu()
  {
   do{
      std::cout<<"\n\tEnter your uol e-mail: ";
      std::cin>>str;
          
      is_val = new uol(str);
         
      if(is_val->isValid())
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
      else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
          
     }while(!is_val->isValid());
  }
  
 private:
  
  bool isValid()
  {
   std::string server("@uol.com.br");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
  }
  
  std::string str;
  validation* is_val; 
  std::string _mail; 
};

struct bol: validation
{
 bol(){}
 
 bol(std::string mail): _mail(mail){}
 
 ~bol()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
  bool isValid()
  {
   std::string server("@bol.com.br");
  
   std::string piece(_mail.substr(_mail.size()-server.size()));
  
   return !piece.compare(server);
  }
 
  void menu()
  {
   do{
      std::cout<<"\n\tEnter your bol e-mail: ";
      std::cin>>str;
          
      is_val = new bol(str);
         
      if(is_val->isValid())
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
      else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
          
     }while(!is_val->isValid());
  }
  
 private:
  std::string str;
  validation* is_val;
  std::string _mail; 
};

struct earth: validation
{
 earth(){}
 
 earth(std::string mail): _mail(mail){}
 
 ~earth()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
  bool isValid()
  {
   std::string server("@terra.com.br");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
  }
  
  void menu()
  {
   do{
      std::cout<<"\n\tEnter your earth e-mail: ";
      std::cin>>str;
          
      is_val = new earth(str);
         
      if(is_val->isValid())
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
      else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
          
     }while(!is_val->isValid());
  }
  
 private:
  std::string str;
  validation* is_val;
  std::string _mail; 
};

struct yahoo: validation
{
 yahoo(){}
 
 yahoo(std::string mail): _mail(mail){}
 
 ~yahoo()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
  bool isValid()
  {
   std::string server("@yahoo.com.br");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
  }
 
  void menu()
  {
   do{
      std::cout<<"\n\tEnter your yahoo e-mail: ";
      std::cin>>str;
          
      is_val = new yahoo(str);
         
      if(is_val->isValid())
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
      else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
          
     }while(!is_val->isValid());
  }
  
 private:
  std::string str;
  validation* is_val;
  std::string _mail; 
};

struct hotmail: validation
{
 hotmail(){}
 
 hotmail(std::string mail): _mail(mail){}
 
 ~hotmail()
  {
   if(!_mail.empty())_mail.clear();  
  }
  
  bool isValid()
  {
   std::string server("@hotmail.com.br");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
  }
  
  void menu()
  {
   do{
      std::cout<<"\n\tEnter your hotmail e-mail: ";
      std::cin>>str;
          
      is_val = new hotmail(str);
         
      if(is_val->isValid())
       std::cout<<"\n\tEmail: " << str << " is Valid.\n\n";
      else 
       std::cout<<"\n\tEmail: " << str << " is Invalid.\n\n";
          
     }while(!is_val->isValid());
  }
  
 private:
  std::string str;
  validation* is_val;
  std::string _mail; 
};

void menu_All()
{
 int op;
 validation* is_val[6];
 
 enum { Gmail=1, Uol, Bol, Earth, Yahoo, Hotmail, Exit };
 
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
      
       is_val[0] = new gmail();
         
       is_val[0]->menu();
      
      break;
      
      case Uol:
       
       is_val[1] = new uol();
       
       is_val[1]->menu();
      
      break;
      
      case Bol:
         
       is_val[2] = new bol();
         
       is_val[2]->menu();
      
      break;
      
      case Earth:
       
       is_val[3] = new earth();
         
       is_val[3]->menu();
      
      break;
      
      case Yahoo:
       
       is_val[4] = new yahoo();
         
       is_val[4]->menu();
       
      break;
      
      case Hotmail:
         
       is_val[5] = new hotmail();
         
       is_val[5]->menu();
      
      break;
      
      case Exit:
       std::cout << "\n\tGood Bye!\n\n";
       exit(1);
      break;
      
      default:
       std::cout << "\n\tInvalid Option!\n\n";
      break; 
     }
   }while(1);
}