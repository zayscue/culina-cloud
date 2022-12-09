type GetAccessTokenCallbackOptions = {
  audience: string;
};

type GetAccessTokenCallback = (
  options: GetAccessTokenCallbackOptions
) => Promise<string>;

export default GetAccessTokenCallback;