import { useState } from 'react'
import {
  Conversation,
  ConversationContent,
  ConversationScrollButton,
} from '@/components/ui/shadcn-io/ai/conversation'
import {
  Message,
  MessageAvatar,
  MessageContent,
} from '@/components/ui/shadcn-io/ai/message'

export function ChatConversation() {
  const [messages] = useState<any[]>([
    {
      id: '1',
      role: 'assistant',
      content: 'Hi there 👋 How can I help you today?',
    },
  ])

  return (
    <div className="flex justify-center w-full px-4">
      {/* Centered chat container without border */}
      <div className="flex flex-col w-full max-w-3xl h-[calc(100vh-220px)] bg-background overflow-hidden">
        <Conversation className="flex-1 overflow-y-auto p-4">
          <ConversationContent>
            {messages.map((message) => (
              <Message key={message.id} from={message.role}>
                <MessageContent>{message.content}</MessageContent>
                <MessageAvatar
                  name={message.role === 'user' ? 'You' : 'AI'}
                  src={''}
                />
              </Message>
            ))}
          </ConversationContent>
          <ConversationScrollButton />
        </Conversation>
      </div>
    </div>
  )
}
