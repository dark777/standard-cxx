#ifndef _EMAIL_HXX
#define _EMAIL_HXX

#include <regex>
#include <iostream>
#include "contact.hxx"

struct email
{  
 email(Contact&);
 
 email(std::string);
 
 ~email();
 
 bool isMail();
   
  private:
  Contact e;
  std::string _email;
};

#endif