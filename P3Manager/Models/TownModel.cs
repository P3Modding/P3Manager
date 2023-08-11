using P3Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.Models;

public class TownModel : INotifyPropertyChanged
{
    public TownModel(TownId id, Storage storage)
    {
        this.Id = id;
        this.Storage = storage;
    }

    public TownId Id { get; set; }
    public Storage Storage { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}
