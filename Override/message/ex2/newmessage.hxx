#include "messagesender.hxx"

class NewMessageCoverter : public MessageCoverter
{
 public:
 std::string convert(std::string msg)
 {
  msg = "<START> " + msg + " <END>";
  return msg;
 }
};  
