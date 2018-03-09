#include <regex>
#include <iostream>

class email
{ 
 struct Contact
 {
  std::string eMail;
 };
 
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
  
  const std::regex pattern("^([a-zA-Z0-9._]+@((?:hotmail|terra|yahoo|uol|bol)[.](?:com[.]br))?((?:gmail)[.](?:com))?)$");
  
  return std::regex_match(_mail, email_smatch, pattern);
 }
 
 std::string print()
 {
  return "\n\tEmail: "+_mail+(isMail()?" is Valid\n":" is Invalid\n");
 }
 
 Contact getContact()
 {
  Contact c;
  
  do{
     std::cout << "\n\tEnter email: ";
     getline(std::cin, c.eMail);
     
     std::cout << email(c).print() << "\n";
     
    }while(!email(c).isMail());
 
  return c;
 }
 
 void getEmail()
 {
  do{
     std::cout << "\n\tEnter email: ";
     getline(std::cin, _mail);
     
     std::cout << email(_mail).print() << "\n";
     
    }while(!email(_mail).isMail());
 }
 
}email;

int main(void)
{
 //email.getEmail();
 email.getContact();
 std::cout << "\n";
 return 0;
}