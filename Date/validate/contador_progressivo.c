#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
//contador progressivo
int main()
{
 int mm = 0;
 
 int sec=0;
 
 int min=0;
 
 int hr=0;

 int i=0;

 //for(int i = 0; i < 5; --i)
 while(1)
  {
   printf("\n\t%02dh:%02dm:%02ds",hr,min,sec);
   //sleep(1000);
   system("clear");
   
   mm++;
   if(mm == 285) // faz com que os segundos sejan contados no mesmo tempo que o relogio do computador
   {
    mm=0;
    sec++;
   }
   
   if(sec == 60)
    {
     sec=0;
     min++;
    }
    
   if(min == 60)
    {
     min=0;
     hr++;
    }
                   
   if(hr == 12)
    {
     hr=0;
    }
  }
 return 0;
}  
