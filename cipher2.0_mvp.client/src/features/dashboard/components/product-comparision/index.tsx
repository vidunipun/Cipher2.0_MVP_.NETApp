import { useState, useEffect } from 'react'
import { Plus, X } from 'lucide-react'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { AnalyticsChart } from '../analytics-chart'

export function ProductComparision() {
  const [selectedProducts, setSelectedProducts] = useState<
    { id: number; name: string }[]
  >([])
  const [availableHeight, setAvailableHeight] = useState<number>(0)

  useEffect(() => {
    // Calculate available height dynamically
    const updateHeight = () => {
      const reservedHeight = 320 // Adjust this to leave room for bottom sections and chat bar
      setAvailableHeight(window.innerHeight - reservedHeight)
    }
    updateHeight()
    window.addEventListener('resize', updateHeight)
    return () => window.removeEventListener('resize', updateHeight)
  }, [])

  const handleAddProduct = () => {
    if (selectedProducts.length >= 10) return
    const newProduct = {
      id: Date.now(),
      name: `Product ${selectedProducts.length + 1}`,
    }
    setSelectedProducts([...selectedProducts, newProduct])
  }

  const handleRemoveProduct = (id: number) => {
    setSelectedProducts(selectedProducts.filter((p) => p.id !== id))
  }

  return (
    <div className='flex min-h-screen flex-col space-y-4 p-4'>
      {/* ✅ Selected Products Section */}
      <Card
        className='flex flex-col transition-all'
        style={{
          height: availableHeight > 0 ? `${availableHeight * 1.0}px` : '50vh', // dynamically set ~100% of available space
        }}
      >
        <CardHeader>
          <CardTitle>Selected Products</CardTitle>
          <CardDescription>
            Add up to 10 products to compare across brands
          </CardDescription>
        </CardHeader>

        <CardContent className='flex-1 overflow-hidden'>
          <div
            className='flex w-full items-stretch justify-start gap-4 transition-all'
            style={{
              flexWrap: 'nowrap',
              height: '100%', // fill card
            }}
          >
            {/* Product cards */}
            {selectedProducts.map((product) => (
              <div
                key={product.id}
                className='bg-muted/50 relative flex flex-col items-center justify-center rounded-lg border text-center shadow-sm transition-all'
                style={{
                  flex: `1 1 ${100 / (selectedProducts.length + 1)}%`,
                  minWidth: '100px',
                  height: '100%',
                  padding: '1rem',
                }}
              >
                <button
                  onClick={() => handleRemoveProduct(product.id)}
                  className='bg-muted hover:bg-destructive absolute top-2 right-2 rounded-full p-1 transition hover:text-white'
                >
                  <X size={14} />
                </button>
                <div className='text-sm font-semibold'>{product.name}</div>
                <div className='text-muted-foreground mt-4 text-xs'>
                  Product details here...
                </div>
              </div>
            ))}

            {/* Add Product button */}
            {selectedProducts.length < 10 && (
              <div
                className='bg-background hover:bg-muted/40 flex cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed transition'
                onClick={handleAddProduct}
                style={{
                  flex: `1 1 ${100 / (selectedProducts.length + 1)}%`,
                  minWidth: '140px',
                  height: '100%',
                }}
              >
                <Plus className='text-muted-foreground h-6 w-6' />
                <span className='text-muted-foreground mt-2 text-sm'>
                  Add Product
                </span>
              </div>
            )}
          </div>
        </CardContent>
      </Card>

      {/* ✅ Bottom sections */}
      <div className='grid grid-cols-1 gap-4 lg:grid-cols-8'>
        <Card className='col-span-1 lg:col-span-4'>
          <CardHeader>
            <CardTitle>Pricing</CardTitle>
            <CardDescription>
              Pricing variations across the selected products
            </CardDescription>
          </CardHeader>
          <CardContent className='px-6'>
            <AnalyticsChart />
          </CardContent>
        </Card>

        <Card className='col-span-1 lg:col-span-4'>
          <CardHeader>
            <CardTitle>Performance</CardTitle>
            <CardDescription>
              Performance variations across the selected products
            </CardDescription>
          </CardHeader>
          <CardContent className='px-6'>
            <AnalyticsChart />
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
