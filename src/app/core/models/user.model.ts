export interface UserDto {
  id: number;
  userName: string | null;
  firstName: string | null;
  lastName: string | null;
  isActive: boolean;
  roleId: number;
  roleName: string | null;
  email: string | null;
  phone: string | null;
  title: string | null;
}