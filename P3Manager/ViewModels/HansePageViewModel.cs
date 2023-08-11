using CommunityToolkit.Mvvm.ComponentModel;
using P3Api;
using P3Manager.Models;
using P3Manager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Manager.ViewModels;

public class DummyTown
{
    public String Id { get; set; } = "test";
    public int[] Wares;

    public DummyTown() {
        this.Wares = new int[20];
    }
}
public class HansePageViewModel : ObservableObject
{
    private ObservableCollection<TownModel> towns = new();
    private List<DummyTown> dummyTowns;

    public HansePageViewModel() {
        // var poll = this.Poll();
        this.dummyTowns = new List<DummyTown>();
        for (int i = 0; i< 20; i++)
        {
            dummyTowns.Add(new DummyTown());
        }
    }

    public ObservableCollection<TownModel> Towns
    {
        get { return this.towns; }
        set { this.towns = value; }
    }

    public List<DummyTown> DummyTowns
    {
        get { return this.dummyTowns; }
    }

    private async Task Poll()
    {
        await Task.Delay(2000);
        while (true)
        {
            Town?[] newData = P3PollService.Data;
            int townsIndex = 0;
            for (int i = 0; i < 40; i++)
            {
                if (newData[i] == null)
                {
                    // Town is not set, we might have to remove it
                    if (townsIndex < this.towns.Count && (int)this.towns[townsIndex].Id == i)
                    {
                        this.towns.RemoveAt(townsIndex);
                    }
                }
                else
                {
                    // Town is set, we might have to add it
                    if (townsIndex < this.towns.Count && (int)this.towns[townsIndex].Id == i)
                    {
                        // Update
                        if (this.towns[townsIndex].Id != newData[i]!.Id)
                        {
                            throw new Exception();
                        }
                        this.towns[townsIndex].Storage = newData[i]!.Storage;
                        //this.towns[townsIndex].OnPropertyChanged("Storage");
                    }
                    else
                    {
                        // Add
                        this.towns.Insert(townsIndex, new((TownId) i, newData[i]!.Storage));
                    }
                    townsIndex += 1;
                }
            }
            // OnPropertyChanged(nameof(this.Towns));
            OnPropertyChanged(nameof(this.Towns));
            await Task.Delay(1000000);
        }
    }
}
