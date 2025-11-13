import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Separator } from '@/components/ui/separator'

export function Profile() {
  // Example mock user data (you can replace this with actual user state or API data)
  const user = {
    username: 'vidunipun.rajapaksha',
    fullName: 'Vidunipun Rajapaksha',
    email: 'vidunipun@example.com',
    role: 'Product Analyst',
    entitlements: ['Compare Products', 'Access Reports', 'View Analytics'],
    assignedBrands: [
      { name: 'L’Oréal', id: 1 },
      { name: 'Garnier', id: 2 },
      { name: 'Maybelline', id: 3 },
    ],
  }

  return (
    <div className='mx-auto flex max-w-5xl flex-col gap-6 p-4 lg:p-6'>
      {/* Profile Card */}
      <Card>
        <CardHeader className='flex flex-col items-center text-center'>
          <Avatar className='h-20 w-20'>
            <AvatarImage
              src={`https://api.dicebear.com/8.x/initials/svg?seed=${user.fullName}`}
              alt={user.fullName}
            />
            <AvatarFallback>{user.fullName.charAt(0)}</AvatarFallback>
          </Avatar>
          <CardTitle className='mt-3 text-lg font-semibold'>
            {user.fullName}
          </CardTitle>
          <CardDescription>@{user.username}</CardDescription>
          <p className='text-muted-foreground mt-1 text-sm'>{user.email}</p>
          <Badge variant='secondary' className='mt-3'>
            {user.role}
          </Badge>
        </CardHeader>
      </Card>

      {/* Entitlements */}
      <Card>
        <CardHeader>
          <CardTitle>Entitlements</CardTitle>
          <CardDescription>
            Features and permissions granted to you
          </CardDescription>
        </CardHeader>
        <CardContent className='flex flex-wrap gap-2'>
          {user.entitlements.map((entitlement, index) => (
            <Badge key={index} variant='outline' className='text-sm'>
              {entitlement}
            </Badge>
          ))}
        </CardContent>
      </Card>

      {/* Assigned Brands */}
      <Card>
        <CardHeader>
          <CardTitle>Assigned Brands</CardTitle>
          <CardDescription>Brands you are responsible for</CardDescription>
        </CardHeader>
        <CardContent className='space-y-3'>
          {user.assignedBrands.map((brand) => (
            <div
              key={brand.id}
              className='hover:bg-muted/30 flex items-center justify-between rounded-md border p-3 transition'
            >
              <div>
                <p className='font-medium'>{brand.name}</p>
                <p className='text-muted-foreground text-xs'>
                  Brand ID: {brand.id}
                </p>
              </div>
              <Button variant='secondary' size='sm'>
                View Details
              </Button>
            </div>
          ))}
        </CardContent>
      </Card>

      {/* Divider */}
      <Separator />

      {/* Action Section */}
      <div className='flex justify-end'>
        <Button variant='destructive'>Log out</Button>
      </div>
    </div>
  )
}
