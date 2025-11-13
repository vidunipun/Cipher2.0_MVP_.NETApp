import { Star } from 'lucide-react'
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from '@/components/ui/card'
import ProductImageCarousel from '../ProductImageCarousel'

export default function ProductDetailView() {
  const product = {
    name: 'Us Polo MeMen Regular Fit Solid Spread Collar Casual Shirt',
    description:
      'Bank Offer10% instant discount on SBI Credit Card EMI Transactions, up to ₹1,500 on orders of ₹5,000 and aboveT&C',
    price: '$249.99',
    link: 'https://www.acme.com/products/smartwatch-x200',
    image:
      'https://images.unsplash.com/photo-1517059224940-d4af9eec41e5?w=1600&auto=format&fit=crop&q=80',
    attributes: {
      Brand: 'U.S. POLO ASSN. ',
      Category: 'Mens shirts',
      Specifications: 'Fits. Regular',
      Size: 'M',
      ReturnPolicy: '10 days',
    },
    topSellingPoints: [
      'Bank Offer10% instant discount on SBI Credit Card',
      'Buy 1 Get 1 free',
      'Bank Offer5% cashback on Flipkart SBI Credit Card',
      'Combo OfferBuy 2 items save 5%; Buy 3 save 7%',
    ],
    rating: 4.5,
    totalReviews: 1280,
  }

  return (
    <div className='bg-background flex min-h-screen w-full flex-col space-y-10 px-6 py-10'>
      {/* ===== Top Two-Column Section ===== */}
      <div className='mx-auto grid w-full max-w-7xl grid-cols-1 gap-8 lg:grid-cols-2'>
        {/* === Left Column: Image === */}
        <div className='flex items-center justify-center'>
          <div className='bg-muted flex aspect-square w-full max-w-md items-center justify-center overflow-hidden rounded-xl border'>
            <img
              src={product.image}
              alt={product.name}
              className='h-full w-full rounded-lg object-cover'
            />
          </div>
        </div>

        {/* === Right Column: Details === */}
        <div className='flex flex-col space-y-6'>
          <Card className='shadow-md'>
            <CardHeader>
              <CardTitle className='text-2xl font-semibold'>
                {product.name}
              </CardTitle>
              <CardDescription>
                Comprehensive product-specific analyzed information
              </CardDescription>
            </CardHeader>
            <CardContent className='space-y-3'>
              <p className='text-muted-foreground text-sm leading-relaxed'>
                {product.description}
              </p>
              <div className='flex items-center gap-4 text-sm'>
                <span className='text-lg font-semibold'>{product.price}</span>
                <a
                  href={product.link}
                  target='_blank'
                  rel='noopener noreferrer'
                  className='text-primary underline'
                >
                  View Product
                </a>
              </div>
            </CardContent>
          </Card>

          {/* Product Attributes */}
          <Card className='shadow-sm'>
            <CardHeader>
              <CardTitle className='text-base font-medium'>
                Product Attributes
              </CardTitle>
            </CardHeader>
            <CardContent>
              <dl className='grid grid-cols-1 gap-x-8 gap-y-3 text-sm sm:grid-cols-2'>
                {Object.entries(product.attributes).map(([key, value]) => (
                  <div key={key} className='flex justify-between border-b py-1'>
                    <dt className='text-muted-foreground'>{key}</dt>
                    <dd className='font-medium'>{value}</dd>
                  </div>
                ))}
              </dl>
            </CardContent>
          </Card>

          {/* Selling Points */}
          <Card className='shadow-sm'>
            <CardHeader>
              <CardTitle className='text-base font-medium'>
                Top Selling Points
              </CardTitle>
            </CardHeader>
            <CardContent>
              <ul className='text-muted-foreground list-disc space-y-1 pl-5 text-sm'>
                {product.topSellingPoints.map((point, i) => (
                  <li key={i}>{point}</li>
                ))}
              </ul>
            </CardContent>
          </Card>

          {/* Ratings */}
          <div className='flex items-center gap-1'>
            {[...Array(5)].map((_, i) => (
              <Star
                key={i}
                className={`h-5 w-5 ${
                  i < Math.floor(product.rating)
                    ? 'fill-yellow-400 text-yellow-400'
                    : 'text-muted-foreground'
                }`}
              />
            ))}
            <span className='ms-2 text-sm font-medium'>
              {product.rating} / 5 ({product.totalReviews} reviews)
            </span>
          </div>
        </div>
      </div>
      {/* ===== Bottom Section: Product Carousel ===== */}
      <div className='mx-auto w-full max-w-7xl py-10'>
        <h2 className='mb-4 text-xl font-semibold'>Explore Related Products</h2>
        <ProductImageCarousel />
      </div>
    </div>
  )
}
