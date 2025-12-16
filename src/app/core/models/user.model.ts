export interface UserDto {
  id: number;
  userName: string | null;
  firstName: string | null;
  lastName: string | null;
  isActive: boolean;
  roleId: number;
  email: string | null;
  phone: string | null;
  title: string | null;
  createdAt: Date | null;
  updatedAt: Date | null;
  deletedAt: Date | null;
}

export interface UserRoleDto {
  id: number;
  roleName: string;
}

export interface UserWithRoleDto extends UserDto {
  role: UserRoleDto;
}