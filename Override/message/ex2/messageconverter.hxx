#include <iostream>
//Message Converter Class
class MessageCoverter
{
 public:
 //virtual std::string convert(std::string msg) //Message Sent :: <START> Hello World <END>  in newmessage.hxx
   
 std::string convert(std::string msg) //Message Sent :: [START]Hello World[END] in newmessage.hxx
 {
  msg = "[START]" + msg + "[END]";
  return msg;
 }
}; 
