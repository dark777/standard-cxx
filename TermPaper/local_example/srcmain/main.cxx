/* TermPaper Header */
#ifndef _SRCMAIN_CXX
#define _SRCMAIN_CXX


#include "../include/termpaper"
//#include "../include/TermPaper/include/termpaper.hxx"
//#include "../include/TermPaper/src/termpaper.cxx"
int main()
{
 TermPaper *trmppr = new TermPaper();

  trmppr->GetDetails();
  trmppr->PrintDetails();
    
 return 0;
} 

#endif