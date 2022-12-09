type PaginatedListViewModel<T> = {
  items: Array<T>
  page: number;
  totalCount: number;
  totalPages: number;
};

export default PaginatedListViewModel;