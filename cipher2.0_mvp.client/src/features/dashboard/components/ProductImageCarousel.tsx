import { faker } from '@faker-js/faker'
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from '@/components/ui/carousel'

export const title = 'Standard Carousel'

const slides = Array.from({ length: 5 }, (_, index) => ({
  id: index + 1,
  image: faker.image.urlPicsumPhotos({ width: 800, height: 400 }),
}))

const ProductImageCarousel = () => (
  <div className='mx-auto w-full max-w-4xl'>
    <Carousel>
      <CarouselContent>
        {slides.map((slide) => (
          <CarouselItem key={slide.id} className='pl-2 md:pl-4'>
            <div className='bg-muted flex aspect-video w-full items-center justify-center overflow-hidden rounded-lg border'>
              <img
                src={slide.image}
                alt={`Slide ${slide.id}`}
                className='h-full w-full object-cover'
              />
            </div>
          </CarouselItem>
        ))}
      </CarouselContent>
      <CarouselPrevious />
      <CarouselNext />
    </Carousel>
  </div>
)

export default ProductImageCarousel
