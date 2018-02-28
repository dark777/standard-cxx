#ifndef _CBUFF_HXX
#define _CBUFF_HXX










#ifdef _cplusplus

extern "C" void cbuff(void);

#else

void cbuff(void);

#endif  //_cplusplus

#if defined(__linux__) || defined(__gnu_linux__)

 #include <stdio_ext.h>
 
#elif defined(_WIN32) || defined(_WIN64) || defined(__WINDOWS__)

 #include <stdio.h>

#endif












#endif  //_CBUFF_HXX