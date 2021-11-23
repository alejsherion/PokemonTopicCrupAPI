namespace WebAPICrudPokemon.DTO;


public class PagintaionResultDTO<T>
{
    public int Page { get; set; }
    public int Records { get; set; }
    public int PagesCount { get; set; }
    public IEnumerable<T> Result { get; set; }
}
