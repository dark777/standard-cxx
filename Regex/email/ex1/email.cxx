#include <regex>
#include <iostream>

struct email
{
 email(std::string mail): _email(mail){}
 
 ~email()
  {
   if(_email.length() != 0) _email.clear();
  }
 
 bool isMail()
 {
  std::smatch email_smatch;
  
  //valida todos os dominios e servidores de emails
  //const std::regex pattern("^([a-z0-9._]+@[a-z]+([.][a-z]{2,4}){1,2})$");
  
  //valida somente servidores @{hotmail,terra,yahoo,uol,bol} no dominio .com.br e servidor gmail no dominio .com
  const std::regex pattern("^([a-z0-9._]+@(?:(?:hotmail|terra|yahoo|uol|bol)[.](?:com[.]br)?)?(?:(?:gmail)[.](?:com)?)?)$");
   
  return std::regex_match(_email, email_smatch, pattern);
 }
 
 void print()
 {
   std::cout<<"\n\tEmail: "<<_email<<(isMail()?" is Valid\n":" is Invalid\n"); 
 }
 
 private:
  std::string _email;
};


int main(void)
{ 
  std::string emails[15] = {
                            "regex_cpp@net.br",
                            "regex_cpp@terra.com.br",
                            "regex_cpp@hotmail.com.br.net",
                            "regex_cpp@hotmail.com",
                            "regex_cpp@yahoo.com.br",
                            "regex_cpp@gmail.com",
                            "reg_ex.cpp@gmail.com.br",    
                            "regex_cpp@uol.com.br",
                            "regex_cpp@uol.com",
                            "regex_cpp@bol.com.br",
                            "regex.cpp@bol.com",
                            "reg_ex.cpp@org.nz",
                            "reg_ex.cpp@net.org.br",
                            "regex_email@terra.com",
                            "regex_email@terra.com.br"    
                            };
  for(int i=0; i<15; i++)
   email(emails[i]).print();
    std::cout<<"\n";
  return 0;
}