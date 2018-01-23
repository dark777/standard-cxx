#ifndef _EMAIL_CXX
#define _EMAIL_CXX

#include "email.hxx"

   email::email(Contact& em): _email(em.email){}
 
   email::email(std::string mail): _email(mail){}
 
   email::~email()
   {
    if(_email.length() != 0)_email.clear();
   }
   
   bool email::isMail()
   {
    std::smatch email_smatch;
  
    const std::regex pattern("([a-zA-Z0-9._]+@(?:(?:hotmail|terra|yahoo|bol|uol)[.](?:com[.]br)?)?)?");
   
    return std::regex_match(_email, email_smatch, pattern);
   }
#endif