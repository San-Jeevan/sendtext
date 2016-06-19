using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iOS.Table;
using UIKit;
using Xamarin.Contacts;

namespace iOS
{
    class NavBarDelegate : UINavigationBarDelegate
    {
        public override UIBarPosition GetPositionForBar(IUIBarPositioning barPositioning)
        {
            return UIBarPosition.TopAttached;
        }
    }

    public partial class AddParticipantController : UIViewController
    {
        UISearchBar searchBar;
        AddParticpantTableSource tableSource;
        public List<string> indexedTableItems;
        public AddParticipantController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

    
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UINavigationItem item = new UINavigationItem();
            item.Title = "Select participant";
            UIBarButtonItem backbutton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Done, CancelClicked);
            item.LeftBarButtonItem = backbutton;
            var rightbtn = new UIBarButtonItem(UIBarButtonSystemItem.Action, MoreOptionsClicked);
            item.RightBarButtonItem = rightbtn;
          Toolbar.PushNavigationItem(item, true);
            Toolbar.Delegate = new NavBarDelegate();


            //Initiate contacts table
            List<string> contacts = new List<string>();
            var book = new Xamarin.Contacts.AddressBook();
            book.RequestPermission().ContinueWith(t => {
                if (!t.Result)
                {
                    Console.WriteLine("Permission denied by user or manifest");
                    return;
                }
                foreach (Contact contact in book.OrderBy(c => c.LastName))
                {
                    if (string.IsNullOrEmpty(contact.FirstName) && string.IsNullOrEmpty(contact.LastName)) continue;
                    if (contact.Phones.FirstOrDefault() == null) continue;
                    contacts.Add(String.Format("{0} {1} ({2})", contact.FirstName, contact.LastName,
                        contact.Phones.FirstOrDefault().Number));

                }
                contacts.Sort((x, y) => x.CompareTo(y));
                 indexedTableItems = contacts.Distinct().ToList();
                tableSource = new AddParticpantTableSource(contacts.Distinct().ToArray(), this);
                ContactsList.Source = tableSource;
            }, TaskScheduler.FromCurrentSynchronizationContext());


            searchBar = new UISearchBar();
            searchBar.Placeholder = "Search";
            searchBar.SizeToFit();
            searchBar.AutocorrectionType = UITextAutocorrectionType.No;
            searchBar.AutocapitalizationType = UITextAutocapitalizationType.None;
            searchBar.SearchButtonClicked += (sender, e) => {
            Search();
            };
            //ContactsList.TableHeaderView = searchBar;

        }

        private void MoreOptionsClicked(object sender, EventArgs e)
        {
            
        }

        void Search()
        {
            var temptable = indexedTableItems.Where(x => x.IndexOf(searchBar.Text, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            indexedTableItems.AddRange(temptable);
            for (int i = indexedTableItems.Count; i-- > 0;)
            {
                if (indexedTableItems.Count == temptable.Count)
                {
                    break;
                }
                indexedTableItems.RemoveAt(i);
            }
          
            if (tableSource != null)
            {
                    ContactsList.ReloadData();
            }
            searchBar.ResignFirstResponder();
        }

     

        private void CancelClicked(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

        public void ParticipantSelected(string indexedTableItem)
        {
            
        }
    }
}