import { type ChangeEvent, useState } from 'react'
import { SlidersHorizontal } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import { Main } from '@/components/layout/main'
import { apps } from './data/apps'

type AppType = 'all' | 'connected' | 'notConnected'

const appText = new Map<AppType, string>([
  ['all', 'All Brands'],
  ['connected', 'Favorites'],
  ['notConnected', 'Not Favorites'],
])

export function LandingPage() {
  const [sort, setSort] = useState<'asc' | 'desc'>('asc')
  const [appType, setAppType] = useState<AppType>('all')
  const [searchTerm, setSearchTerm] = useState('')

  const [brands, setBrands] = useState(apps)

  const toggleFavorite = (name: string) => {
    setBrands((prev) =>
      prev.map((b) =>
        b.name === name ? { ...b, isFavorite: !b.isFavorite } : b
      )
    )
  }

  const filteredApps = brands
    .sort((a, b) =>
      sort === 'asc'
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name)
    )
    .filter((app) =>
      appType === 'connected'
        ? app.isFavorite
        : appType === 'notConnected'
          ? !app.isFavorite
          : true
    )
    .filter((app) => app.name.toLowerCase().includes(searchTerm.toLowerCase()))

  const handleSearch = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value)
  }

  return (
    <>
      <Main fixed>
        <div>
          <h1 className='text-2xl font-bold tracking-tight'>Featured Brands</h1>
          <p className='text-muted-foreground'>
            Here&apos;s a list of brands for analysis!
          </p>
        </div>

        <div className='my-4 flex items-end justify-between sm:my-0 sm:items-center'>
          <div className='flex flex-col gap-4 sm:my-4 sm:flex-row'>
            <Input
              placeholder='Filter brands...'
              className='h-9 w-40 lg:w-[250px]'
              value={searchTerm}
              onChange={handleSearch}
            />

            <Select
              value={appType}
              onValueChange={(value) => setAppType(value as AppType)}
            >
              <SelectTrigger className='w-36'>
                <SelectValue>{appText.get(appType)}</SelectValue>
              </SelectTrigger>
              <SelectContent>
                <SelectItem value='all'>All Brands</SelectItem>
                <SelectItem value='connected'>Favorites</SelectItem>
                <SelectItem value='notConnected'>Not Favorites</SelectItem>
              </SelectContent>
            </Select>

            <Select
              value={sort}
              onValueChange={(value) => setSort(value as 'asc' | 'desc')}
            >
              <SelectTrigger className='w-16'>
                <SelectValue>
                  <SlidersHorizontal size={18} />
                </SelectValue>
              </SelectTrigger>
              <SelectContent align='end'>
                <SelectItem value='asc'>Ascending</SelectItem>
                <SelectItem value='desc'>Descending</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        <Separator className='shadow-sm' />

        <ul className='no-scrollbar grid gap-4 overflow-auto pt-4 pb-16 md:grid-cols-2 lg:grid-cols-3'>
          {filteredApps.map((app) => (
            <li
              key={app.name}
              className='rounded-lg border p-4 hover:shadow-md'
            >
              <div className='mb-8 flex items-center justify-between'>
                <div className='bg-muted flex size-10 items-center justify-center rounded-lg p-2'>
                  {app.logo}
                </div>

                <Button
                  variant='outline'
                  size='sm'
                  onClick={() => toggleFavorite(app.name)}
                  className={`${
                    app.isFavorite
                      ? 'border border-yellow-400 bg-yellow-50 hover:bg-yellow-100 dark:border-yellow-600 dark:bg-yellow-950 dark:hover:bg-yellow-900'
                      : ''
                  }`}
                >
                  {app.isFavorite ? '★ Favorite' : '☆ Add Favorite'}
                </Button>
              </div>

              <div>
                <h2 className='mb-1 font-semibold'>{app.name}</h2>
                <p className='line-clamp-2 text-gray-500'>{app.desc}</p>
              </div>
            </li>
          ))}
        </ul>
      </Main>
    </>
  )
}
