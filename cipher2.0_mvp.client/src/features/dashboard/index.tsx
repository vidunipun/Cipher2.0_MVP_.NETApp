import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { ConfigDrawer } from '@/components/config-drawer'
import { Header } from '@/components/layout/header'
import { ProfileDropdown } from '@/components/profile-dropdown'
import { Search } from '@/components/search'
import { ThemeSwitch } from '@/components/theme-switch'
import { ChatPromptBar } from './components/Chat-promptBar'
import { AggregateAnalysis } from './components/aggregate-analysis'
import { ChatConversation } from './components/ai-chat'
import { LandingPage } from './components/landing-page'
import { ProductComparision } from './components/product-comparision'
import Products from './components/products'
import { Profile } from './components/profile'

export function Dashboard() {
  return (
    <Tabs
      defaultValue='landingPage'
      orientation='vertical'
      className='bg-background flex min-h-screen flex-col'
    >
      {/* ===== FIXED HEADER (2 rows: top bar + tabs row) ===== */}
      <div className='bg-background fixed top-0 left-0 z-50 w-full border-b'>
        {/* --- Row 1: Title + controls --- */}
        <Header className='bg-background'>
          <div className='flex w-full items-center justify-between px-4 py-3'>
            <h1 className='text-2xl font-bold tracking-tight'>CIPHER 2.0</h1>
            <div className='ms-auto flex items-center space-x-4'>
              <Search />
              <ThemeSwitch />
              <ConfigDrawer />
              <ProfileDropdown />
            </div>
          </div>
        </Header>

        {/* --- Row 2: Tabs list (separate row below header) --- */}
        <div className='bg-background border-border border-t'>
          <div className='flex w-full justify-center overflow-x-auto px-4 py-2'>
            <TabsList>
              <TabsTrigger value='landingPage'>Landing Page</TabsTrigger>
              <TabsTrigger value='products'>Products</TabsTrigger>
              <TabsTrigger value='productComparision'>
                Product Comparision
              </TabsTrigger>
              <TabsTrigger value='aggregateAnalysis'>
                Aggregate Analysis
              </TabsTrigger>
              <TabsTrigger value='aiChat'>AI Chat</TabsTrigger>
              <TabsTrigger value='profile'>Profile</TabsTrigger>
            </TabsList>
          </div>
        </div>
      </div>

      {/* ===== MAIN CONTENT (with top margin for header + tablist height) ===== */}
      <div className='mt-[130px] flex-1 overflow-y-auto px-4 pb-24'>
        <TabsContent value='landingPage'>
          <LandingPage />
        </TabsContent>

        <TabsContent value='products'>
          <Products />
        </TabsContent>

        <TabsContent value='productComparision'>
          <ProductComparision />
        </TabsContent>

        <TabsContent value='aggregateAnalysis'>
          <AggregateAnalysis />
        </TabsContent>

        <TabsContent value='aiChat'>
          <ChatConversation />
        </TabsContent>

        <TabsContent value='profile'>
          <Profile />
        </TabsContent>
      </div>

      {/* ===== FIXED CHAT PROMPT BAR (bottom) ===== */}
      <div className='bg-background sticky bottom-0 z-50 border-t'>
        <ChatPromptBar
          onSend={(message) => {
            console.log('User sent:', message)
          }}
        />
      </div>
    </Tabs>
  )
}
