type PaginatedListAPIResponse<T> = {
  items: Array<T>
  page: number;
  totalCount: number;
  totalPages: number;
};

export default PaginatedListAPIResponse;
