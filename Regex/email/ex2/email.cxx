#include <regex>
#include <iostream>

struct Contact
{
 std::string eMail;
}; 

struct email
{
 email(std::string mail): _mail(mail){}
   
 email(Contact& em): _mail(em.eMail){}
 
 ~email()
  {
   if(_mail.length() != 0)_mail.clear();  
  }
  
 bool isMail()
 {
  std::smatch email_smatch;

  const std::regex pattern("([a-zA-Z0-9._]+@(?:(?:hotmail|terra|yahoo|bol)[.](?:com[.]br)?)?(?:(?:gmail)[.](?:com)?)?)?");

  return std::regex_match(_mail, email_smatch, pattern);
 }

 void print()
 {
  std::cout<<"\n\tEmail: "<<_mail<<(isMail()?" is Valid\n":" is Invalid\n"); 
 }
 
 private:
  std::string _mail;
};

void getEmail()
{
  std::string mail;
  
  do{
     std::cout << "\n\tEnter email: ";
     getline(std::cin, mail);
     
     email(mail).print();
     
    }while(!email(mail).isMail());
}

Contact getContact()
{
 Contact c;
 
  do{
     std::cout << "\n\tEnter email: ";
     getline(std::cin, c.eMail);
     
     email(c).print();
     
    }while(!email(c).isMail());
 
 return c;
}



int main(void)
{
 //getContact();
 getEmail();
 std::cout<<"\n";
 return 0;
}