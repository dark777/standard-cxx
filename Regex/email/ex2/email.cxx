#include <regex>
#include <iostream>

class email
{ 
 struct Contact
 {
  std::string eMail;
 }c;
 
 std::string _mail;
 
 public:
 
 email(){}
 
 email(Contact& em): _mail(em.eMail){}
 
 email(std::string mail): _mail(mail){}
 
 ~email()
  {
   if(_mail.length() != 0)_mail.clear();  
  }
  
 bool isMail()
 {
  std::smatch email_smatch;
  
  const std::regex pattern("([a-zA-Z0-9._]+@(?:(?:hotmail|terra|yahoo|uol|bol)[.](?:com[.]br)?)?(?:(?:gmail)[.](?:com)?)?)?");
  
  return std::regex_match(_mail, email_smatch, pattern);
 }
 
 void print()
 {
  std::cout << "\n\tEmail: " << _mail << (isMail()?" is Valid\n":" is Invalid\n"); 
 }
 
 Contact getContact()
 {
  do{
     std::cout << "\n\tEnter email: ";
     getline(std::cin, c.eMail);
     
     email(c).print();
     
    }while(!email(c).isMail());
 
  return c;
 }
 
 void getEmail()
 {
  do{
     std::cout << "\n\tEnter email: ";
     getline(std::cin, _mail);
     
     email(_mail).print();
     
    }while(!email(_mail).isMail());
 }
 
};

int main(void)
{
 //email().getEmail(); 
 email().getContact();
 std::cout<<"\n";
 return 0;
}