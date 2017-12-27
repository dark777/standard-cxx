/* TermPaper Header */
#ifndef _TERMPAPER_HXX
#define _TERMPAPER_HXX

#include <iostream>

class TermPaper
{
   private:
     std::string fName; // first name
     std::string lName; // last name
     std::string subject; // subject of the paper
     char letterGrade; // assigned letter grade

   public:

    TermPaper();

     std::string getFName();
     std::string getLName();
     std::string getSubject();
     void GetDetails();
     void PrintDetails();
     char getLetterGrade();
     
     void setFName(std::string fN);
     void setLName(std::string lN);
     void setSubject(std::string sub);
     void setLetterGrade(char grade);
};

#if !defined(_TERMPAPER_INC) && defined(_TERMPAPER_HXX) || defined(_TERMPAPER_CXX)
#error" Never include <TermPaper/include/termpaper.hxx> \
or <TermPaper/src/termpaper.cxx> directly; \
use include <termpaper> instead. "
#endif

#include <TermPaper/src/termpaper.cxx>

#endif  