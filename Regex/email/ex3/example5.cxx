#include <iostream>

//base class validation
struct validation
{
 validation(){}
 ~validation(){}
 //pure virtual function to enforce reimplementation
 virtual void menu() = 0;
 virtual bool isValid() = 0;
 
 protected:
  std::string str; 
  validation* is_val;
};

//function foward declaration
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
   delete is_val; 
   _mail.clear(); 
  }
  
  bool isValid()
  {
   std::string server("@gmail.com");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
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
  std::string _mail; 
};

struct uol: validation
{
 uol(){}
 
 uol(std::string mail): _mail(mail){}
 
 ~uol()
  {
   delete is_val; 
   _mail.clear(); 
  }
  
  bool isValid()
  {
   std::string server("@uol.com.br");
   
   std::string piece(_mail.substr(_mail.size()-server.size()));
   
   return !piece.compare(server);
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
  std::string _mail; 
};

struct bol: validation
{
 bol(){}
 
 bol(std::string mail): _mail(mail){}
 
 ~bol()
  {
   delete is_val; 
   _mail.clear();
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
  std::string _mail; 
};

struct earth: validation
{
 earth(){}
 
 earth(std::string mail): _mail(mail){}
 
 ~earth()
  {
   delete is_val; 
   _mail.clear();
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
  std::string _mail; 
};

struct yahoo: validation
{
 yahoo(){}
 
 yahoo(std::string mail): _mail(mail){}
 
 ~yahoo()
  {
   delete is_val; 
   _mail.clear();
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
  std::string _mail; 
};

struct hotmail: validation
{
 hotmail(){}
 
 hotmail(std::string mail): _mail(mail){}
 
 ~hotmail()
  {
   delete is_val; 
   _mail.clear();
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
  std::string _mail; 
};

void menu_All()
{
 int op;
 validation* m_val;
 
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
       
       m_val = new gmail();
         
       m_val->menu();
       
      break;
      
      case Uol:
       
       m_val = new uol();
       
       m_val->menu();
       
      break;
      
      case Bol:
         
       m_val = new bol();
         
       m_val->menu();
       
      break;
      
      case Earth:
       
       m_val = new earth();
         
       m_val->menu();
       
      break;
      
      case Yahoo:
       
       m_val = new yahoo();
         
       m_val->menu();
       
      break;
      
      case Hotmail:
         
       m_val = new hotmail();
         
       m_val->menu();
       
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