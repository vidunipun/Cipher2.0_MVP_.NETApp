import { SetStateAction, useState } from 'react'
import {
  PromptInput,
  PromptInputTextarea,
  PromptInputToolbar,
  PromptInputSubmit,
} from '@/components/ui/shadcn-io/ai/prompt-input'

type ChatPromptBarProps = {
  onSend: (message: string) => void
}

export function ChatPromptBar({ onSend }: ChatPromptBarProps) {
  const [input, setInput] = useState('')

  return (
    <div className='bg-background border-t p-4'>
      <PromptInput
        onSubmit={(e: { preventDefault: () => void }) => {
          e.preventDefault()
          const message = input.trim()
          if (!message) return

          onSend(message)
          setInput('')
        }}
        className='flex items-center gap-2'
      >
        <PromptInputTextarea
          value={input}
          onChange={(e: { currentTarget: { value: SetStateAction<string> } }) =>
            setInput(e.currentTarget.value)
          }
          placeholder='Type your message…'
          autoFocus
        />

        <PromptInputToolbar>
          <PromptInputSubmit disabled={!input.trim()} />
        </PromptInputToolbar>
      </PromptInput>
    </div>
  )
}
